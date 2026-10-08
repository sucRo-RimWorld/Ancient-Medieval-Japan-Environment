Feature: AMJE development-source loaded regression

  Scenario: Development production Mod and assembly are the expected source root
    Then the development Environment Mod and assembly come from the expected source folder

  Scenario: Four authored plant Defs have unique development ownership
    Then all four Environment plants are uniquely owned by the expected source Mod

  Scenario: Japan-oriented biome plant exclusion is loaded from development source
    Then rejected Vanilla vegetation is absent from the selected Japanese biomes

  Scenario: Replacement native plants retain tuned development commonality
    Then the replacement structural plants preserve their balanced commonalities

  Scenario: Native wood harvest Defs match the active Vanilla or Medieval Overhaul profile
    Then the harvested wood Def contracts remain correct with or without Medieval Overhaul
