#!/bin/sh

echo "Looking for merge conflicts..."
echo "---"
grep --color=always -r -e "<<<<<<< HEAD" Monopoly/Assets

