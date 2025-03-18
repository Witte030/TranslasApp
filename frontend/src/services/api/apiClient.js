// Basic API client with error handling

async function handleResponse(response) {
    let data;
    
    try {
      // Try to parse as JSON, but handle case where there's no content
      const text = await response.text();
      data = text ? JSON.parse(text) : {};
    } catch (error) {
      throw new Error('Unable to parse server response');
    }
    
    if (!response.ok) {
      throw new Error(data.message || `Error ${response.status}: ${response.statusText}`);
    }
    
    return data;
  }
  
  export default {
    async get(url) {
      const response = await fetch(url);
      return handleResponse(response);
    },
    
    async post(url, data) {
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
      });
      
      return handleResponse(response);
    },
    
    async put(url, data) {
      const response = await fetch(url, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
      });
      
      return handleResponse(response);
    },
    
    async delete(url) {
      const response = await fetch(url, {
        method: 'DELETE'
      });
      
      return handleResponse(response);
    }
  };