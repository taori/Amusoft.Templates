# ACTIONNAME
Write some description and explain what the action does

## Usage
The action can be used like this:

```yml
on: [push]
      
jobs:
  build:
    name: Build 
    runs-on: ubuntu-latest
      
    steps:

    - uses: GITOWNER/GITPROJECT@v1
      with:
        input1: someinput
```