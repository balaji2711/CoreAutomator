@Websocket
Feature: Websocket
	Tests a web socket connection

@Sanity
Scenario: Check Websocket connection
	When I tests a valid socket connection
	Then connection should be established