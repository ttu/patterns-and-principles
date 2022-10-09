# C# Patterns & Principles

Code examples for _Patterns and Principles - are these important anymore?_ training.

Main aim of the training was to show how languages have evolved from the times when Design Pattern were often required and why and when some patterns should still be used.

Examples are in C# and some have JavaScript implementation to show how this could be implemented by using functions only. 

Examples try to use more real life cases, but sometimes it is hard to come up with a simple example.

Examples also contain UML class diagram code samples, as Gof Patterns are presented in UML. 

Each file has classes and interfaces for the pattern/principle and tests for executing examples.

## Patterns

### Gang of Four

GoF Pattern class diagrams: [Design Patterns Quick Reference (MCDONALDLAND)](
http://www.mcdonaldland.info/2007/11/28/40/)

#### Behavioral

  * Command
  * Observer
  * Memento
  * Strategy
  * State
  * Iterator
    * Check implemenation from: 
      * https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/iterators
      * https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerable.getenumerator?view=netcore-2.2    
  * __TODO__
    * Template Method
    * Visitor
  * __TODO MAYBE__
    * Chain of Responsibility
    * Interpreter
    * Iterator
    * Mediator

#### Creational

  * Prototype
  * Singleton
  * Object Pool
  * Factory / Factory Method
  * Abstract Factory
  * Builder

#### Structural

  * Adapter
  * Composition
  * Decorator
  * Proxy
  * Facade
  * __TODO MAYBE__
    * Bridge
    * Flyweight

### Other

These examples mainly compare Functional and Object patterns.

  * Dependecy Injection
  * Functions instead of DI
  * OOP to Functions
  * OOP vs FP modules
  * Class vs closure
  * Fluent interface / method chaining
  * __TODO__
	  * Service Locator
	  * Repository
      * Unit of work

## Principles

### SOLID Principles

  * Liskov substitution
  * Interface segregation
  * __TODO__
    * Single responsibility
    * Open/Closed
    * Dependency inversion

### Other
 
  * IoC Container
  * Coupling and Cohesion
  * __TODO__
    * Modularity 
    * Command Query separation

## Links

* https://sourcemaking.com/design_patterns
* http://www.mcdonaldland.info/2007/11/28/40/
* https://github.com/Lc5/DesignPatternsCSharp
* https://github.com/abishekaditya/DesignPatterns

#### Differences

* https://softwareengineering.stackexchange.com/questions/178488/lsp-vs-ocp-liskov-substitution-vs-open-close
* https://softwareengineering.stackexchange.com/questions/201397/difference-between-the-adapter-pattern-and-the-proxy-pattern
* https://stackoverflow.com/questions/1658192/what-is-the-difference-between-strategy-design-pattern-and-state-design-pattern
 * https://stackoverflow.com/questions/17937755/what-is-the-difference-between-a-fluent-interface-and-the-builder-pattern*