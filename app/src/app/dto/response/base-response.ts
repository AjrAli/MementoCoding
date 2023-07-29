export class BaseResponse {
    message: string;
    success: boolean;
    constructor(){
        this.message = '';
        this.success = true;
    }
}