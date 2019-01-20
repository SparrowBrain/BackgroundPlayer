Feature: SkinPool
	In order to know what skins I can use
	As an end-user
	I want to see all loaded skins

Scenario: Skin pool lists all available skins
	Given the app started
	When I press the Settings button
	Then I should see all skins listed with names

Scenario: Skin pool shows details for selected skin
	Given the app started
	When I press the Settings button
		And I click on an individual skin
	Then I should see duration and offset for the skin