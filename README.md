Logs:
```
Passed!  - Failed:     0, Passed:     4, Skipped:     0, Total:     4, Duration: 13 s - TestingWithSelenium.NUnit.dll (net8.0)

Passed!  - Failed:     0, Passed:     4, Skipped:     0, Total:     4, Duration: 3 s - TestingWithSelenium.xUnit.dll (net8.0)
```

### Test Results Comparison

| **Runner** | **Passed** | **Failed** | **Skipped** | **Total** | **Duration** |
|------------|------------|------------|-------------|-----------|--------------|
| **xUnit**  | 4          | 0          | 0           | 4         | 3 seconds    |
| **NUnit**  | 4          | 0          | 0           | 4         | 13 seconds   |

### Conclusion

xUnit runner performed much better, as expected tho, because it was built with the idea to be ran in parallel. (Better utilization of multithreading) Also xUnits uses constructors for setting up and tearing down while the NUnit relies on seperate methods.