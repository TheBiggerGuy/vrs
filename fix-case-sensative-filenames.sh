#!/bin/bash

set -o errexit          # Exit on most errors (see the manual)
set -o errtrace         # Make sure any error trap is inherited
set -o nounset          # Disallow expansion of unset variables
set -o pipefail         # Use last non-zero exit code in a pipeline
#set -o xtrace          # Trace the execution of the script (debug)

root="${PWD}"
for resfile in */*.resx ; do
    resfilereal=$(realpath ${resfile})
    resfiledir=$(dirname ${resfile})
    echo "Found Resex file ${resfile}" #(resfilereal=${resfilereal} resfiledir=${resfiledir}"

    cd "${resfiledir}"
    find * -type f -print0 | sed -e 's/\//\\\\/g' | sed -e 's/\./\\./g' | xargs -0 --no-run-if-empty -I{} sed --in-place 's/<value>{};/<value>{};/gI' "${resfilereal}"
    cd "${root}"
done

git status --porcelain --untracked-files=no | grep "^ M" | grep -F ".resx" | cut -d' ' -f 3 | xargs unix2dos