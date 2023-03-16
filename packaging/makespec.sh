#!/bin/bash

SCRIPT_FILE=$(readlink -f $0)
SCRIPT_DIR=$(dirname $SCRIPT_FILE)

VERSION_FILE=$SCRIPT_DIR/version.txt
RPMSPEC=$SCRIPT_DIR/csapi-flux.spec
RPMSPEC_IN=$RPMSPEC.in

source $VERSION_FILE

while getopts ":r:" opt; do
  case $opt in
    r) RPM_VERSION=$OPTARG ;;
    :) echo "Option -$OPTARG requires an argument."; exit 1 ;;
  esac
done

if [ -z "$RPM_VERSION" ]; then
  echo "-r option for rpm version is required."
  echo "eq. makespec.sh -r 0.0.1"
  exit 1
fi

# Update RPM Spec
echo "# Auto-generated from $(basename $RPMSPEC_IN) by makespec.sh" | cat - $RPMSPEC_IN > $RPMSPEC
sed -i -e "s/@rpm_version@/$RPM_VERSION/g" $RPMSPEC
