#!/bin/bash

# http://blog.fossasia.org/deploying-a-kivy-application-with-pyinstaller-for-mac-osx-to-github/
# https://github.com/fossasia/kniteditor/blob/master/.travis.yml
# https://github.com/fossasia/kniteditor/tree/master/mac-build

set -e

HERE="`dirname \"$0\"`"
USER="$1"
cd "$HERE"

echo "# brew --cache"
brew --cache
echo "# brew update"
brew update

echo "# install python3"
brew install python3
echo -n "Python version: "
python3 --version
python3 -m pip install --upgrade pip

python3 -m pip install six
python3 -m pip install pycparser

python3 setup.py build_ext --inplace
