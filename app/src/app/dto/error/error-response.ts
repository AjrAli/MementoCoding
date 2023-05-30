export interface ErrorResponse {
  message: string;
  validationErrors: string[];
}
export function harmonizeErrorResponse(response: any): ErrorResponse {
  const harmonizedResponse: ErrorResponse = {
    message: '',
    validationErrors: []
  };

  if (response.hasOwnProperty('message') && response.message) {
    harmonizedResponse.message = response.message;
  } else if (response.hasOwnProperty('Message') && response.Message) {
    harmonizedResponse.message = response.Message;
  }

  if (response.hasOwnProperty('validationErrors') && response.validationErrors) {
    harmonizedResponse.validationErrors = response.validationErrors;
  } else if (response.hasOwnProperty('ValidationErrors') && response.ValidationErrors) {
    harmonizedResponse.validationErrors = response.ValidationErrors;
  }

  // Ajoutez ici les autres propriétés à harmoniser

  return harmonizedResponse;
}