# Description
Point-and-click adventure game about a child exploring a mystical town and forest in 19th-century eastern europe trying to find their missing sister.

# Git LFS
The repository implements [Git LFS](https://www.atlassian.com/git/tutorials/git-lfs) to handle large files (assets).
`.gitattributes` contains the LFS tracking configuration.
### Important Commands
* Pull or clone LFS files as a batch
``` git lfs pull ```
``` git lfs clone ```
* Track new filetype in any directory
``` git lfs track "*.ogg" ```

# Folder structure
## Code
Code lives in Assets/Scripts. 
* /Core: Core features like dialogue and save system
* /Features: Specific, but essential features like inventory and movement
* /Interactions: Scripts managing interactions between player, items, objects and NPCs
* /Puzzles: Separate puzzles, each containing a main script inheriting `PuzzleManager.cs`

## Assets