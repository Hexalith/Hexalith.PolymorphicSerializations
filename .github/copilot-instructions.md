# AI assistant instructions

Before working in this repository, read
[`hexalith-llm-instructions.md`](../references/Hexalith.AI.Tools/hexalith-llm-instructions.md)
(in the `references/Hexalith.AI.Tools` submodule) and follow it.

## Git Submodules

- Initialize root-declared submodules only, using the `references/...` paths declared in the root `.gitmodules` file.
- Avoid recursive submodule commands unless they are explicitly scoped so that nested submodules are not initialized.
- If nested submodules are initialized accidentally, deinitialize them before continuing.

## Module-specific AI instructions

Please also read and follow the instructions in
`references/Hexalith.Builds/CLAUDE.md` for coding standards, build commands,
and project conventions.

Before working on module user interface or UX, read and follow
`references/Hexalith.AI.Tools/hexalith-ux-instructions.md`.
