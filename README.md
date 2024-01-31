# TTS File Cleaner


## Description
This C# script is designed to clean text files created for Text-to-Speech (TTS) purposes, specifically for news articles obtained from web browsers. The goal is to remove unwanted content such as headers, menus, footers, and other irrelevant text, ensuring that only the actual news article is processed by the TTS reader.


## Functionality
Configuration via INI file: The script utilizes an INI configuration file (settings.ini) to customize its behavior. Users can specify parameters such as the minimum character count to retain a line and the maximum Levenshtein distance for similarity between lines.

**User Input:** The script prompts the user to input the filepath of the TTS text file that needs cleaning.

**Remove Short Lines:** Lines in the TTS file with a character count below a specified threshold (configured in the INI file) are removed. The script informs the user about each removed line.

**Remove Similar Lines:** Identical and very similar consecutive lines are removed based on the Levenshtein distance between them (configured in the INI file). This step helps eliminate duplicated or nearly duplicated content.

**Levenshtein Distance Calculation:** The script includes a Levenshtein distance calculation function (ComputeLevenshteinDistance) to determine the similarity between two strings. This function is used in the process of removing similar lines.
Output Cleaned File: The cleaned content is saved to a new text file with "_cleaned" appended to the original filename. The file is stored in the same directory as the original TTS file.

**Console Output:** Throughout the process, the script provides console output, including information about removed lines and the Levenshtein distances calculated.


## Usage
Run the script.

Enter the filepath of the TTS text file to be cleaned.

Monitor the console output for information about removed lines and the cleaning process.

Find the cleaned text file in the same directory as the original file.


## Dependencies
The script relies on the **IniParser library** for parsing the INI configuration file.


## Note
Users are encouraged to customize the settings.ini file based on their preferences for cleaning TTS text files.
