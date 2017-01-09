#!/bin/bash

# http://blog.fossasia.org/deploying-a-kivy-application-with-pyinstaller-for-mac-osx-to-github/
# https://github.com/fossasia/kniteditor/blob/master/.travis.yml
# https://github.com/fossasia/kniteditor/tree/master/mac-build

wget https://repo.continuum.io/miniconda/Miniconda3-latest-MacOSX-x86_64.sh
Miniconda-latest-MacOSX-x86_64.sh -b

echo -n "Python version: "
python --version
python -m pip install --upgrade pip

python -m pip install six
python -m pip install pycparser

python setup.py build_ext --inplace
