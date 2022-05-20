@Get
Feature: GetRequest
	Verify the get request

@Sanity
Scenario: Check the get request
	When I run the user request API
	Then verify the success response from user API