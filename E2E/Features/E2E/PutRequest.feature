@Put
Feature: PutRequest
	Verify the put request

@Sanity
Scenario: Check the put request - User API
	When I run the put call 
	Then verify the response from put call