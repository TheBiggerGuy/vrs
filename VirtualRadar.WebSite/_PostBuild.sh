#!/bin/sh

# This shell script copies the content of the Web folder to a folder called Web under the VirtualRadar
# build folder. It destroys existing content in the build folder's version of Web.
#
# It also builds the Checksums.txt file and copies it into position in the VirtualRadar build folder.
#
# The arguments to the script are:
# %1 = $(SolutionDir)
# %2 = $(ProjectDir)
# %3 = $(ConfigurationName)

set SOLUTIONDIR="${1}"
set PROJECTDIR="${2}"
set CONFIGNAME="${3}"

# Build the checksums file directly into the VirtualRadar build folder
set CHKSUMEXE="${SOLUTIONDIR}ThirdParty/ChecksumFiles/bin/${CONFIGNAME}/ChecksumFiles.exe"
set CHKSUMROOT="${PROJECTDIR}Site/Web"
set CHKSUMFILE="${SOLUTIONDIR}VirtualRadar/bin/x86/${CONFIGNAME}/Checksums.txt"

# Check checksum program exists
if not exist "%CHKSUMEXE%"; then
    echo "FAILED: The checksum executable \"${CHKSUMEXE}\" does not exist."
    exit 1
fi

echo "Generating checksums of web content"
echo "%CHKSUMEXE%" -root:"%CHKSUMROOT%" -out:"%CHKSUMFILE%" \
     "%CHKSUMEXE%" -root:"%CHKSUMROOT%" -out:"%CHKSUMFILE%" -addContentChecksum
if not errorlevel 1 goto CHKSUMWORKED
echo FAILED: Could not generate checksum file "%CHKSUMFILE%", errorlevel is %ERRORLEVEL%
exit 1
:CHKSUMWORKED

# The unit tests on the web site need to have the checksums file in a fixed location, i.e.
# its folder should not include the configuration name
xcopy /QY "%CHKSUMFILE%" "%PROJECTDIR%Site"

# Copy the web content into the Web folder under the VirtualRadar build folder
set CPSOURCE=%PROJECTDIR%Site\Web
set CPDEST=%SOLUTIONDIR%VirtualRadar\bin\x86\%CONFIGNAME%\Web

if exist "%CPSOURCE%\" goto SOURCEOK
    echo FAILED: The source folder "%CPSOURCE%" does not exist.
    exit 1
:SOURCEOK

if not exist "%CPDEST%\" goto DESTMISSING
    echo Erasing "%CPDEST%"
    rmdir /s /q "%CPDEST%"
    if not errorlevel 1 goto DESTMISSING
    echo FAILED: Could not #ove the "%CPDEST%" folder, errorlevel is %ERRORLEVEL%
    exit 1
:DESTMISSING

echo Copying "%CPSOURCE%" to "%CPDEST%"
xcopy /EQYI "%CPSOURCE%" "%CPDEST%"
if not errorlevel 0 goto :BADWEBCOPY
    echo Copied web site output to VirtualRadar build folder
    goto CPOK
:BADWEBCOPY
    echo FAILED: The copy failed with errorlevel %ERRORLEVEL%
    exit 1
:CPOK

# Exit
exit 0
