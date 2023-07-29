import { BaseResponse } from "../base-response";

export class ErrorResponse extends BaseResponse {
  validationErrors: string[];
  constructor(){
    super();
    this.validationErrors = [];
  }
}