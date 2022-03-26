#!/bin/sh

echo "Looking for meta file..."
echo "---"
grep --color=always -r -e "$1" Monopoly/Assets/**/**.meta

