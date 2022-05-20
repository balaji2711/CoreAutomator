@Delete
Feature: DeleteRequest
	Verify the delete request

@Sanity
Scenario: Check the delete request - User API
	When I run the delete call 
	Then verify the response from delete call