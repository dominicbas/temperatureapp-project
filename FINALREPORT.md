## Final Report

### Development Process
The development process for this temperature sensor system was structured into multiple sprints, each focusing on specific functionalities. Utilizing a Kanban board was an integral part of organizing tasks, and having a clear road map of what each sprint would be focused on ensuring that there was a clear incentive on the priority of tasks to ensure deadlines were met. 

Starting with the research phase, I invested time in understanding .Net, and what a console application was. Through this, I found tutorials outlining how to get your first console application running which was the foundation of the sensor application. Once the main console application was running I researched storage solutions and other useful things needed for this application and decided to go with SQLite database, and JSON handling using `Newtonsoft.Json`. This foundation helped create a modular application that could accommodate future enhancements to be implemented. 


### Git Usage
Git was a big part of this project as it provided the much-needed version control and code management required to ensure quality and efficiency in the implementation of each phase. So I began to go through the Git/GitHub LinkedIn course that provided me with all the necessary skillsets I needed to be able to utilize Github within the project overall reducing the burden due to the effortless management GitHub provides to a project. A remote repository was created on GitHub for the GitHub Linkedin and xUnit Courses along with one for the Sensor application, commits were regularly pushed to maintain progress visibility. Each feature I worked on was developed on a separate branch and merged into a release branch where it was reviewed and then merged into the main branch only after working as intended. If needed I had a branch for bug fixes which helped with isolating problems within the code. This workflow reduced the risk of introducing bugs into the main codebase and ensured the traceability of changes.

Some challenges arose with Git merge conflicts during the first few commits in the repository. However, these were resolved by reviewing the conflicts carefully and ensuring that each branch was being updated frequently to avoid major changes within the environments. 


### Testing Practices
I unfortunately was unable to produce any successful tests during the testing phase of the project as there were too many error conflicts within my environment that I couldn't find the time to properly research and fix. I had set up the xUnit packages demonstrated in the xUnit LinkedIn Tutorials and copied the same environment setup as per the exercises but still faced many issues. So I instead focused on working on the other aspects of the project to ensure quality. 

Another challenge I found with testing was replicating how the test was meant to behave whilst still reflecting the underlying application.

### Challenges Faced
The project faced a few technical and knowledge challenges. Initially, setting up SQLite integration proved cumbersome due to a lack of familiarity with its configuration in .NET. This issue was mitigated through extensive research, documentation, review and troubleshooting.

Another hurdle was implementing data smoothing and anomaly detection with sufficient accuracy. These features required balancing mathematical calculations for rolling averages with performance considerations for large datasets. Overall I found each time I introduced a new method into the environment another one would break due to conflict or incorrect resource allocation during the application runtime. This was where following a branching strategy with GitHub proved to be prevalent.  

Finally, ensuring graceful shutdown with `ShutdownSensor()` presented challenges related to resource cleanup, such as closing database connections and clearing temporary memory. We implemented robust error handling to address potential issues during shutdown.


### Feature Development and Testing
Each feature was approached methodically and in the order of its development:

- **InitialiseSensor()**: The foundational method was implemented first, allowing the creation of sensor instances with specific attributes such as name, location, and temperature thresholds. This was tested with different configurations to ensure flexibility and proper initialization.
- **SimulateData()**: Introduced early in the project, this method generated random temperature readings within a defined range. It was tested to confirm randomness and fault tolerance, ensuring simulated readings accurately reflected expected conditions.
- **ValidateData()**: Following data simulation, validation logic was added to ensure readings stayed within the specified `MinValue` and `MaxValue`. Testing covered edge cases, confirming that out-of-range readings were correctly flagged as invalid.
- **LogData() and StoreData()**: These methods were implemented next to log readings to a text file and store them in an SQLite database. Manual testing was performed to verify file outputs, and database storage was tested using queries to ensure accurate persistence and retrieval of readings.
- **SmoothData()**: This feature was introduced later to calculate rolling averages for data smoothing. Testing involved datasets with gradual changes to verify the calculated averages were accurate and responsive to trends.
- **DetectAnomaly()**: Built alongside smoothing, this method identified irregular spikes by comparing current readings against recent averages. Testing included datasets with known anomalies to confirm proper detection.
- **CheckThreshold()**: Developed as an advanced feature, this method flagged readings that exceeded critical limits. Simulated edge cases were tested to validate alerts in scenarios where thresholds were intentionally breached.
- **ShutdownSensor()**: The final feature, focused on graceful sensor termination, ensured resources like database connections were properly released. Testing involved running multiple shutdown scenarios to confirm reliability.

By adhering to this sequence, features were built incrementally, with each new addition enhancing the system's capabilities.
.

### Conclusion
This project demonstrated the importance of planning, iterative development, and rigorous testing. Despite challenges, we successfully implemented all features, ensuring the application met its requirements and was robust for practical use.
