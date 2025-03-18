# Domain-Driven Design Refactoring Roadmap

## Phase 1: Preparation and Planning
- [ ] Identify core domain concepts (Card, Supplier, Receiver, Carrier)
- [ ] Map current business rules and validations
- [ ] Create a domain glossary to standardize terminology
- [ ] Document current application flow and dependencies

## Phase 2: Domain Model Foundation
- [ ] Create base Entity and AggregateRoot classes
- [ ] Implement ValueObject classes for complex attributes
- [ ] Build domain entity models for Card, Supplier, Receiver, Carrier
- [ ] Add basic validation rules to domain models

## Phase 3: Repository Layer
- [ ] Create repository interfaces for each domain entity
- [ ] Implement API repositories that implement these interfaces
- [ ] Add caching mechanisms for frequently accessed data
- [ ] Write unit tests for repositories

## Phase 4: Application Services
- [ ] Create application services for each domain area
- [ ] Move business logic from components to services
- [ ] Implement command and query patterns
- [ ] Add validation and error handling

## Phase 5: UI Integration
- [ ] Create view models to connect domain models with UI
- [ ] Update components to use application services instead of direct API calls
- [ ] Implement form handling with domain validation
- [ ] Add reactive state management with Pinia/Vuex

## Phase 6: Testing and Refinement
- [ ] Write unit tests for domain models and validation
- [ ] Add integration tests for application services
- [ ] Review and optimize performance
- [ ] Document architecture for team members


src/
├── domain/                     # Domain layer - business rules and entities
│   ├── models/                 # Domain models (entities and value objects)
│   │   ├── common/             # Base classes and shared models
│   │   │   ├── Entity.js       # Base entity class
│   │   │   ├── AggregateRoot.js # Root entity that ensures consistency
│   │   │   └── ValueObject.js  # Immutable value objects
│   │   ├── card/               # Card domain
│   │   │   ├── Card.js         # Card entity
│   │   │   ├── CardStatus.js   # Value object for card status
│   │   │   └── CardValidation.js # Validation rules for cards
│   │   ├── supplier/           # Supplier domain
│   │   │   └── Supplier.js     # Supplier entity
│   │   ├── receiver/           # Receiver domain
│   │   │   └── Receiver.js     # Receiver entity
│   │   └── carrier/            # Carrier domain
│   │       └── Carrier.js      # Carrier entity
│   ├── services/               # Domain services (complex business logic)
│   │   ├── CardService.js      # Card-specific business rules
│   │   └── ValidationService.js # Cross-entity validation rules
│   └── repositories/           # Repository interfaces (contracts)
│       ├── ICardRepository.js  # Card repository interface
│       ├── ISupplierRepository.js # Supplier repository interface
│       └── IEntityRepository.js # Generic repository interface
│
├── application/                # Application layer - orchestrates use cases
│   ├── services/               # Application services (use cases)
│   │   ├── CardApplicationService.js  # Card use cases
│   │   ├── SupplierApplicationService.js # Supplier use cases
│   │   └── EntityApplicationService.js # Generic entity use cases
│   ├── commands/               # Command objects
│   │   ├── card/               # Card commands
│   │   │   ├── CreateCardCommand.js # Command to create a card
│   │   │   └── UpdateCardCommand.js # Command to update a card
│   │   └── entity/             # Entity commands
│   │       ├── CreateEntityCommand.js # Command to create an entity
│   │       └── UpdateEntityCommand.js # Command to update an entity
│   └── queries/                # Query objects
│       └── entity/             # Entity queries
│           └── GetEntitiesQuery.js # Query to get entities
│
├── infrastructure/             # Infrastructure layer - technical capabilities
│   ├── api/                    # API clients
│   │   ├── ApiClient.js        # Base API client
│   │   ├── endpoints.js        # API endpoint configuration
│   │   └── errorHandling.js    # API error handling
│   ├── repositories/           # Repository implementations
│   │   ├── ApiCardRepository.js # Card repository implementation
│   │   ├── ApiSupplierRepository.js # Supplier repository implementation
│   │   └── ApiEntityRepository.js # Generic repository implementation
│   └── services/               # Infrastructure services
│       ├── LoggingService.js   # Logging service
│       └── StorageService.js   # Local storage service
│
├── presentation/               # Presentation layer (optional separate folder)
│   ├── components/             # Vue components
│   ├── composables/            # Vue composables
│   ├── stores/                 # Pinia/Vuex stores
│   └── views/                  # Vue views/pages
│
├── assets/                     # Static assets
├── locales/                    # i18n translations
└── App.vue                     # Root component


2. Core Domain Concepts
Domain Models
At the heart of Domain-Driven Design are your domain models. These are rich objects that encapsulate both data and behavior:

Entity: Objects with identity and lifecycle (e.g., Card, Supplier)
Value Object: Immutable objects without identity (e.g., Address, Priority)
Aggregate: Cluster of entities and value objects with a root entity
Repository: Provides data access abstraction for aggregates
Example: Entity Base Class
An Entity base class serves as a foundation for all domain entities:

// Description: Base class for all entities, providing common functionality
// Responsibilities:
// - Provide identity management
// - Track entity state changes
// - Implement equality comparison

Example: Domain Service
Domain services handle operations that don't naturally belong to a single entity:

// Description: Validates cross-entity business rules
// Responsibilities:
// - Check for entity name uniqueness across system
// - Validate relationships between entities
// - Apply complex business rules

3. Implementation Steps in Detail

Phase 1: Preparation and Planning
Identify Core Domain Concepts

Review existing code to identify entities (Card, Supplier, etc.)
Document relationships between entities
List all business rules (e.g., "entity names must be unique")
Create Domain Glossary

Define consistent terminology (what exactly is a "Card", "Supplier", etc.)
Ensure all team members understand these terms the same way
Document Current Flow

Map how data flows through your application
Identify tightly coupled components that need refactoring
Phase 2: Domain Model Foundation
Create Base Classes

Entity: Base for all entities with identity
ValueObject: Immutable objects without identity
AggregateRoot: Entities that are access points to aggregates
Implement Domain Models

Start with simple models without complex logic
Add validation rules gradually
Ensure models are properly encapsulated
Business Rules Implementation

Add methods to enforce business rules within models
Create domain services for rules spanning multiple entities
Phase 3: Repository Layer
Define Repository Interfaces

Create contracts for data access
Keep interfaces focused on domain concepts, not technical details
Implement Repository Classes

Create implementations that use your API
Handle technical concerns (caching, error handling)
Phase 4: Application Services
Create Application Services

These orchestrate use cases by coordinating domain objects
Handle transaction boundaries
Apply application-specific logic
Command and Query Pattern

Commands: Represent intentions to change state
Queries: Request information without changing state
Phase 5: UI Integration
ViewModels

Create adapter objects between domain models and UI
Handle UI-specific formatting and state
Component Refactoring

Update components to use application services
Remove direct API calls from components
Implement proper state management
4. Implementation Strategies
Incremental Approach
Start Small: Begin with a single feature or domain area
Vertical Slicing: Implement complete features end-to-end
Parallel Implementation: Keep old code working while building new architecture
Feature Flags: Use flags to toggle between old and new implementations
Testing Strategy
Domain Model Testing: Unit test business rules and validation
Integration Testing: Test repositories and application services
UI Testing: Test components with mocked services
5. Common Pitfalls to Avoid
Overengineering: Don't create complex structures for simple problems
Anemic Models: Avoid models that are just data containers without behavior
Inconsistent Terminology: Use consistent language across the application
Leaky Abstractions: Don't let infrastructure concerns leak into domain models
Tight Coupling: Keep layers properly separated
6. Example Migration Path
First Feature: Start with the "Entity Management" feature

Create domain models for basic entities
Implement repository pattern for these entities
Update components to use new architecture
Second Feature: Move to "Card Management"

More complex domain with relationships
Implement proper validation and business rules
Shared Components: Refactor shared components last

Update to work with both old and new architecture
Gradually phase out old implementations
7. Benefits of This Architecture
Maintainability: Clear separation of concerns
Testability: Domain logic can be tested independently
Flexibility: Infrastructure can change without affecting domain
Scalability: Team members can work on different layers
Business Alignment: Code structure mirrors business concepts
Conclusion
Moving to a Domain-Driven Design architecture is a significant investment that pays off in maintainability and flexibility. By following this incremental approach, you can gradually transform your application while continuing to deliver value to users.

Remember, the goal is not to implement a perfect architecture all at once, but to continuously improve your codebase in a direction that supports your business needs and development workflow.

====================================================================================================

# Color Function Fixes

## Replace all instances of lighten() and darken() with color.adjust():

1. Search across all SCSS files for:
   - `lighten($color-*, ` → `color.scale($color-*, $lightness: `
   - `darken($color-*, ` → `color.scale($color-*, $lightness: -`

2. For specific percentage conversions:
   - `lighten($color, 5%)` → `color.scale($color, $lightness: 10%)`
   - `lighten($color, 10%)` → `color.scale($color, $lightness: 20%)`
   - `lighten($color, 20%)` → `color.scale($color, $lightness: 35%)`
   - `lighten($color, 40%)` → `color.scale($color, $lightness: 67%)`
   - `lighten($color, 45%)` → `color.scale($color, $lightness: 75%)`
   - `darken($color, 5%)` → `color.scale($color, $lightness: -10%)`
   - `darken($color, 10%)` → `color.scale($color, $lightness: -20%)`
   - `darken($color, 20%)` → `color.scale($color, $lightness: -35%)`

The `color.scale()` function is more intuitive than `color.adjust()` for percentage-based scaling.

# Color Consistency ToDo List

## High Priority
- [x] Update _supplier-management.scss to use color.scale() instead of lighten()/darken()
- [ ] Create a color-system.scss file as a single source of truth for colors
- [ ] Update _variables.scss to extend or import from color-system.scss
- [ ] Fix _home.scss and _language-switcher.scss to use Sass variables instead of CSS variables
- [ ] Create component-specific mixins for common styles

## Medium Priority
- [ ] Update all button hover states to use a consistent darkening amount
- [ ] Standardize message/alert styles across components
- [ ] Ensure form element styles are consistent (inputs, selects, textareas)
- [ ] Implement consistent focus states across interactive elements

## Low Priority
- [ ] Create a color palette documentation page for developers
- [ ] Add accessibility checks to ensure color contrast ratios meet WCAG standards
- [ ] Optimize color variables with CSS custom properties for runtime theming
- [ ] Consider implementing a dark mode theme