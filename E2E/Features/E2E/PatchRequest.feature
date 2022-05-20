@Patch
Feature: PatchRequest
	Verify the patch request

@Sanity
Scenario: Check the patch request - User API
	When I run the patch call 
	Then verify the response from patch call