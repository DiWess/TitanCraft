# Task Start Procedure

Use this repeatable process for every Project Director task.

1. Define the task in one sentence using the exact human request.
2. Run preflight:

   ```bash
   python3 tools/agent_preflight.py "<exact task>"
   ```

3. Read the routed files, memories, skills, and checklists named by the packet.
4. State forbidden scope before editing, including gameplay, scene, asset, or CI limits.
5. Implement the minimal change needed for the requested slice.
6. Run validation commands that match the changed files.
7. Produce evidence: changed file list, command results, artifacts, hashes, screenshots, or review notes as applicable.
8. Give the final verdict using only the approved vocabulary for the task.
9. Write the PR summary with scope, evidence, tests, manual checks, risks, and final verdict.
