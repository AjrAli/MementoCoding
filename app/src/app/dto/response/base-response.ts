export class BaseResponse {
    message: string;
    success: boolean;
    constructor(message?: string, error?:boolean){
        this.message = message ?? '';
        this.success = error ?? true;
    }
}