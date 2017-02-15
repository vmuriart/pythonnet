import sys

import sysconfig
import struct
import ctypes
import platform
import subprocess

PY_MAJOR = sys.version_info[0]


def _check_output(*args, **kwargs):
    """Check output wrapper for py2/py3 compatibility"""
    output = subprocess.check_output(*args, **kwargs)
    if PY_MAJOR == 2:
        return output
    return output.decode("ascii")


print("UCS Stuff")
# Only works on PY27 on Travis
print(sysconfig.get_config_var('Py_UNICODE_SIZE'))

# Works on all PY27 and all versions after, didn't check PY26
print(ctypes.sizeof(ctypes.c_wchar))

# Works on PY27, wrong on PY33+
unicode_width = 2 if sys.maxunicode < 0x10FFFF else 4
print(unicode_width)
print("####################################################################\n")

print("ARCH Stuff")
# All work
print(struct.calcsize('P') * 8)
is_64bits = sys.maxsize > 2**32
arch = "x64" if platform.architecture()[0] == "64bit" else "x86"
print(is_64bits)
print(arch)
print("####################################################################\n")

print("LIBDIR stuff")
# Works on all python versions tested.
print(sysconfig.get_config_var("LIBDIR"))
print("####################################################################\n")

print("Py_ENABLE_SHARED stuff")
# Returns None on Windows.
print(sysconfig.get_config_var("Py_ENABLE_SHARED"))
lddout = _check_output(["ldd", sys.executable])
print(lddout)
print("####################################################################\n")
