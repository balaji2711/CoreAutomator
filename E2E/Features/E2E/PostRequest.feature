@Post
Feature: PostRequest
	Verify the post request

@Sanity
Scenario: Check the post request - User API
	When I run the post call 
	Then verify the response from post call