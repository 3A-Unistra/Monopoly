#!/bin/sh

echo "Looking for TODO, BUG and FIXME labels..."
echo "---"
grep --color=always -r -e "TODO"  --exclude-dir=Monopoly/Assets/3rdParty Monopoly/Assets
grep --color=always -r -e "BUG"   --exclude-dir=Monopoly/Assets/3rdParty Monopoly/Assets
grep --color=always -r -e "FIXME" --exclude-dir=Monopoly/Assets/3rdParty Monopoly/Assets

