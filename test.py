from argparse import ArgumentParser
from tdw.controller import Controller
from tdw.tdw_utils import TDWUtils
import keyboard


"""
A simple controller to test tdw_launcher.

Requirements:

- Python:
    1. tdw
    2. keyboard
- tdw repo
- tdw_launcher application

Usage:

1. cd path/to/tdw/Python
2. python3 freeze.py --controller path/to/tdw_launcher/test.py
3. <run tdw_launcher>
4. Enter a number and press OK.

Note: Replace path/to with actual paths.

Result: The controller and build launch. A room is created from the number that your input.
Use arrow keys to rotate the camera. Press Esc to quit.
"""


class KeyboardController(Controller):
    def __init__(self, size: int, angle: int = 15, port: int = 1071, display: int = None):
        """
        :param size: The dimensions of the room.
        :param angle: Angle of rotation.
        :param port: Port for the build.
        :param display: Display number (optional).
        """

        super().__init__(port=port, display=display)
        self.angle = angle
        self.done = False
        self.size = size

    def run(self):
        self.start()
        commands = [TDWUtils.create_empty_room(self.size, self.size)]
        commands.extend(TDWUtils.create_avatar(position={"x": 0, "y": 1.5, "z": 0}, avatar_id="a"))
        self.communicate(commands)

        # Listen for keys.
        keyboard.on_press_key("left", self.on_left)
        keyboard.on_press_key("right", self.on_right)
        # Quit the application.
        keyboard.on_press_key("esc", self.on_esc)

        # Continue until the quit key is pressed.
        while not self.done:
            continue
        self.communicate({"$type": "terminate"})

    def on_left(self, e):
        """
        Handle left key press.
        """

        self.communicate([{"$type": "rotate_sensor_container_by",
                           "axis": "yaw",
                           "angle": self.angle * -1,
                           "sensor_name": "SensorContainer",
                           "avatar_id": "a"}])

    def on_right(self, e):
        """
        Handle left key press.
        """

        self.communicate([{"$type": "rotate_sensor_container_by",
                           "axis": "yaw",
                           "angle": self.angle,
                           "sensor_name": "SensorContainer",
                           "avatar_id": "a"}])

    def on_esc(self, e):
        self.done = True


if __name__ == "__main__":
    # Listen for a "size" argument from tdw_launcher's input box.
    parser = ArgumentParser()
    parser.add_argument("size", type=int, default=20)
    args = parser.parse_args()
    KeyboardController(size=args.size).run()
