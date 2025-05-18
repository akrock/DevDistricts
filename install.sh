#!/usr/bin/env bash
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --quality ga --version 9.0.300
export DOTNET_ROOT="$HOME/.dotnet"
export PATH="$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools"

dotnet restore DevDistricts.sln