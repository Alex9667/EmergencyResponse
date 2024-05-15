Feature: ModelTest

@mytag
Scenario: Test the model 
	Given the street name is Fjellerupvej
	And the housenumber is 7
	And the floor is null
	And the door is null
	And the postalcode is 5463
	And the postal code name is Harndrup
	When the address is created
	Then the address should match the inputs