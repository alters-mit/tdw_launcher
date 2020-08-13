from argparse import ArgumentParser
from tdw.keyboard_controller import KeyboardController
from tdw.tdw_utils import TDWUtils
from tdw.output_data import OutputData, Images
from pathlib import Path


"""
A simple controller to test tdw_launcher.
"""


class ExampleController(KeyboardController):
    def __init__(self, size: int, angle: int = 15, port: int = 1071):
        """
        :param size: The dimensions of the room.
        :param angle: Angle of rotation.
        :param port: Port for the build.
        """

        super().__init__(port=port)
        self.angle = angle
        self.done = False
        self.size = size
        self.images_directory = Path("images")
        if not self.images_directory.exists():
            self.images_directory.mkdir()
        self.images_directory = str(self.images_directory.resolve())

    def run(self):
        self.start()
        commands = [TDWUtils.create_empty_room(self.size, self.size)]
        commands.extend(TDWUtils.create_avatar(position={"x": 0, "y": 1.5, "z": 0}, avatar_id="a"))
        commands.extend([{"$type": "set_pass_masks",
                          "avatar_id": "a",
                          "pass_masks": ["_img"]},
                         {"$type": "send_images",
                          "frequency": "always"}])
        self.communicate(commands)

        # Listen for keys.
        # Turn left.
        self.listen(key="left", commands={"$type": "rotate_sensor_container_by",
                                          "axis": "yaw",
                                          "angle": self.angle * -1,
                                          "sensor_name": "SensorContainer",
                                          "avatar_id": "a"})
        # Turn right.
        self.listen(key="right", commands={"$type": "rotate_sensor_container_by",
                                           "axis": "yaw",
                                           "angle": self.angle,
                                           "sensor_name": "SensorContainer",
                                           "avatar_id": "a"})
        # Quit the application.
        self.listen(key="esc", function=self.stop)

        # Continue until the quit key is pressed.
        i = 0
        while not self.done:
            # Listen for keyboard input. Receive output data.
            resp = self.communicate([])
            for r in resp[:-1]:
                r_id = OutputData.get_data_type_id(r)
                # Save images.
                if r_id == "imag":
                    TDWUtils.save_images(images=Images(r),
                                         output_directory=self.images_directory,
                                         filename=TDWUtils.zero_padding(i, width=4))
                    # Increment the image number.
                    i += 1
        self.communicate({"$type": "terminate"})

    def stop(self):
        """
        Stop the simulation.
        """

        self.done = True


if __name__ == "__main__":
    # Listen for a "size" argument from tdw_launcher's input box.
    parser = ArgumentParser()
    parser.add_argument("size", type=int)
    args = parser.parse_args()
    ExampleController(size=args.size).run()
