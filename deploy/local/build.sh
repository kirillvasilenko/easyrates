#!/bin/bash 
 
#CURRENT_TIME=$(date +%s) 


buildService () {
  SVC_NAME=$1
  PRJ_ROOT=$2
  FULL_SVC_NAME="easyrates-$SVC_NAME"
  SVC_VERSION=$(git describe --match "$SVC_NAME*")
  SVC_VERSION=$(echo $SVC_VERSION | cut -d'/' -f 2)
  
  rm -r "$PRJ_ROOT/bin"
  rm -r "$PRJ_ROOT/obj"
  dotnet publish -c Release "$PRJ_ROOT"
  
  docker build -t "$FULL_SVC_NAME" -f "$PRJ_ROOT/Dockerfile" "$PRJ_ROOT"
  docker tag "$FULL_SVC_NAME" "$FULL_SVC_NAME:$SVC_VERSION"
  docker tag "$FULL_SVC_NAME" "$FULL_SVC_NAME:latest"
}

dotnet restore "../../"

buildService "reader" "../../Reader/EasyRates.ReaderApp.AspNet"
buildService "writer" "../../Writer/EasyRates.WriterApp.AspNet"
buildService "migrator" "../../Model/EasyRates.Migrator.Pg"










