Feature: AddressBfe

@mytag
Scenario: Get address bfe
	Given the street is Fjellerupvej
	And the house number is 7
	And the postal code is 5463
	When the address is looked up
	Then the result should be 2705220