@Register
Feature: Register
	Check login scenario in sauce demo application

@Sanity
@Regression
Scenario Outline: Verify the user is able to register into sauce demo application
	When I enter the username <userName> and password <password>
	And I click on logout
	Then I should see user logged out from application successfully

	Examples: 
	| userName        | password       |
	| "standard_user" | "secret_sauce" |