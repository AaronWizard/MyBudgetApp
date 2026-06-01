#!/bin/bash
export SHELL="${SHELL:-/bin/bash}" # Needed for "ng completion" call

dotnet restore API
dotnet ef database update --project API/MyBudgetApp.API
ng completion
