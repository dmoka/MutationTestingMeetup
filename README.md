## Unpick specification
1. Implement an unpick POST methif with route products/{id}/unpick
2. When the product unpicked, change latest pick state to Unpicked
3. When the product unpicked, increase the stock level correspondingly
4. Return BadRequest when stock level reaches 200
5. BadRequest should have error "Cannot unpick more at reached max stock inventory limit"
```dotnet stryker```


## Run stryker
1. Go to test folder "MutationTestingMeetup.Tests"
2. Open PowerShell
3. Run
```dotnet stryker```


## Event
-- agenda
-- mutation testing
--- talk about bread
-- demo MT
-- TDD
-- demo TDD

## DEMO MT
-Talk about Project
- Talk about ProductController and Product about the domain
- TODO: MAKE IMAGE?
- Check code coverage?
-Talk about Stryker
-- Show link https://stryker-mutator.io/
-- shot config file
-- let's run it


## Mutations
- "The product category must be specified" missiing - HasTextInBody
- ProductPicked edge case - add new test
- Again the exception message is not passed
- orderby in product finder -> add multiple products 2nd with Acer, then it is ordered
- ShouldNotReturnExistingProductWhenFilteredOut