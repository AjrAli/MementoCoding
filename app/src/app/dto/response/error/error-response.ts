import { BaseResponse } from "../base-response";

export class ErrorResponse extends BaseResponse {
  validationErrors: string[];
  constructor(message?: string, error?:boolean){
    super(message, error);
    this.validationErrors = [];
  }
}