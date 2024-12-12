Feature: EHU Website User Journeys
  As a visitor of the EHU website
  I want to navigate through various pages and functionalities
  So that I can access the information I need

  Scenario: Navigate to About EHU page
    Given I am on the EHU home page
    When I navigate to the About page
    Then I should see the About page header

  Scenario: Search for study programs
    Given I am on the EHU home page
    When I search for "study programs"
    Then the search results should contain "study programs"

  Scenario: Change language to Lithuanian
    Given I am on the EHU home page
    When I change the language to Lithuanian
    Then the page should be displayed in Lithuanian

  Scenario: Verify Contact Information
    Given I am on the Contacts page
    When I view the Admission Inquiries section
    Then I should see the correct contact information