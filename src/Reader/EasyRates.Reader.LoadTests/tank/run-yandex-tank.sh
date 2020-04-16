#!/usr/bin/env bash

docker rm yandex-tank
docker run --name yandex-tank -v /$(pwd):/var/loadtest -v /$HOME/.ssh:/root/.ssh  -it direvius/yandex-tank