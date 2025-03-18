// Service for handling entity operations (receiver, supplier, carrier)
// This will serve as a bridge between your components and the API

// Responsibilities:
// - Perform API calls for entities
// - Validate entity data before submission
// - Handle common error scenarios
// - Apply business rules (e.g., name uniqueness)

// Next steps after implementation:
// - Add unit tests
// - Extend with more business rules
// - Integrate with a more comprehensive domain model
// services/api/entityService.js
import apiClient from './apiClient';

class EntityService {
  constructor(entityType) {
    this.entityType = entityType;
    this.endpoint = `/api/${entityType}`;
  }
  
  async getAll() {
    return apiClient.get(this.endpoint);
  }
  
  async getById(id) {
    return apiClient.get(`${this.endpoint}/${id}`);
  }
  
  async create(entityData) {
    // Validate before sending to API
    this.validateEntity(entityData);
    
    // Check for duplicate name
    await this.ensureUniqueName(entityData.name);
    
    // If validation passes, create the entity
    return apiClient.post(this.endpoint, entityData);
  }
  
  async update(id, entityData) {
    this.validateEntity(entityData);
    await this.ensureUniqueName(entityData.name, id);
    return apiClient.put(`${this.endpoint}/${id}`, entityData);
  }
  
  async delete(id) {
    return apiClient.delete(`${this.endpoint}/${id}`);
  }
  
  // Business validation rules
  validateEntity(entityData) {
    if (!entityData.name || entityData.name.trim() === '') {
      throw new Error('Name is required');
    }
    
    if (entityData.name.length > 100) {
      throw new Error('Name cannot exceed 100 characters');
    }
    
    // Add more validation rules as needed
  }
  
  // Business rule: ensure name uniqueness
  async ensureUniqueName(name, excludeId = null) {
    try {
      const entities = await this.getAll();
      
      const isDuplicate = entities.some(entity => 
        entity.name.toLowerCase() === name.toLowerCase() && 
        (!excludeId || entity.id !== excludeId)
      );
      
      if (isDuplicate) {
        throw new Error(`A ${this.entityType} with this name already exists`);
      }
    } catch (error) {
      // If we can't check for uniqueness (e.g., API error), rethrow
      if (error.message !== `A ${this.entityType} with this name already exists`) {
        throw new Error(`Unable to verify name uniqueness: ${error.message}`);
      }
      throw error;
    }
  }
}

// Create instances for each entity type
export const receiverService = new EntityService('receiver');
export const supplierService = new EntityService('supplier');
export const carrierService = new EntityService('carrier');

// Generic function to get the right service based on entity type
export function getEntityService(entityType) {
  switch (entityType) {
    case 'receiver': return receiverService;
    case 'supplier': return supplierService;
    case 'carrier': return carrierService;
    default: throw new Error(`Unknown entity type: ${entityType}`);
  }
}