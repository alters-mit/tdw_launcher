# tdw_launcher

`tdw_launcher` is a small application that will launch a controller and a build.

![](tdw_launcher.jpg)

This application will do the following:

1. Prompt the user to input a "code", which is actually a command-line argument for a [TDW controller](https://github.com/threedworld-mit/tdw/blob/master/Documentation/getting_started.md#the-controller).
2. When the user presses the "OK" button, launch your controller, which will launch the build.

## Installation - Backend

As usual with TDW, you will need to write your own controller. However, this controller's code is "frozen", meaning that is a binary executable. This means that the user-end computer doesn't need Python3 or the TDW repo.

You need:

- Windows, OS X, or Linux
- Unity 2019.4 (if you want to modify the launcher application)
- Python3
- The [tdw repo](https://github.com/threedworld-mit/tdw)
- The [`tdw` pip module](https://github.com/threedworld-mit/tdw/blob/master/Documentation/getting_started.md#installation)
- [A compiled executable of tdw_launcher](https://github.com/alters-mit/tdw_launcher/releases/latest)

### Testing

This repo contains a `test.py` controller. This controller will:

- Create a room of a size from an argument (example: `test.py 20` creates a 20x20 room)
- Use left and right arrows to rotate the camera. Esc key to quit.
- Save images per frame:

```
tdw_launcher/
....test.py
....images/
........img_0000.jpg
........img_0001.jpg
```

To use run `test.py` with tdw_launcher, you need to [freeze the controller code](https://github.com/threedworld-mit/tdw/blob/master/Documentation/misc_frontend/freeze.md):

1. `cd path/to/tdw/Python` (replace `path/to` with the path to the `tdw` repo)
2. Freeze the code:

| Windows                                                     | OS X and Linux                                               |
| ----------------------------------------------------------- | ------------------------------------------------------------ |
| `py -3 freeze.py --controller path/to/tdw_launcher/test.py` | `python3 freeze.py --controller path/to/tdw_launcher/test.py` |

(replace `path/to` with the path to the `tdw_launcher` repo

***

Running `freeze.py` will create a binary executable of `test.py` located here: `~/tdw_build/tdw_controller/` (where `~` is your home directory). 

### How tdw_launcher works

Assuming you have a frozen controller binary with the expected filepath, when you run the tdw_launcher application it will:

- Prompt you for a "code". Enter a number, such as 20.
- When you press OK, tdw_launcher will launch the executable found in `~/tdw_build/tdw_controller/` with the text you entered as an argument. For  example: `~/tdw_build/tdw_controller/tdw_controller.exe 20`
- The controller will automatically download the latest [TDW build](https://github.com/threedworld-mit/tdw/blob/master/Documentation/getting_started.md#the-build) to `~/tdw_build/TDW`.
- The controller will launch the build and begin running.

### Writing your own controller

You can use your own controller with tdw_launcher, provided it accepts a single argument:

```python
from argparse import ArgumentParser

parser = ArgumentParser()
parser.add_argument("size", type=int) # You can rename this argument and change its type if needed.
args = parser.parse_args()

print(parser.size)
```

See `test.py` for how `args.size` is used for a very basic scene setup.

Once you have written your controller, [freeze the controller code](https://github.com/threedworld-mit/tdw/blob/master/Documentation/misc_frontend/freeze.md). It will automatically be named and moved to the directory that tdw_launcher expects it to be.

## Installation - Frontend

Any computer that uses tdw_launcher needs the following:

- Windows, OS X, or Linux
- An Internet connection
- [The tdw_launcher application](https://github.com/alters-mit/tdw_launcher/releases/latest)
- Your frozen controller binary executable, located at: `~/tdw_build/tdw_controller/` (where `~` is the computer's home directory). 