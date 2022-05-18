import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ImageService {

  constructor() { }

  // TODO test it

  public convertToBase64(buffers: any[]): string[] {
    let base64Images = [];
    for (var i = 0; i < buffers.length; i++) {
      base64Images.push(this.convertBufferToBase64(buffers[i]));
    }

    console.log("images: ", base64Images);

    return buffers;
  }

  private convertBufferToBase64(buffer: any): string {
    console.log("buffer: ", buffer);
    var binary = '';
    var bytes = new Uint8Array(buffer);
    for (var i = 0; i < bytes.byteLength; i++) {
      binary += String.fromCharCode(bytes[i]);
    }
    let result = btoa(binary);
    console.log("base64: ", result);
    return result;
  }
}
