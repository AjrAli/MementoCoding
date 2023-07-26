import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'boldText'
})
export class BoldTextPipe implements PipeTransform {
  transform(text: string | null, keyword: string | null): string {
    if (!text || !keyword) {
      return '';
    }

    const startIndex = text.toUpperCase().indexOf(keyword.toUpperCase());
    if (startIndex !== -1) {
      const beforeKeyword = text.slice(0, startIndex);
      const keywordText = text.slice(startIndex, startIndex + keyword.length);
      const afterKeyword = text.slice(startIndex + keyword.length);
      return `${beforeKeyword}<strong>${keywordText}</strong>${afterKeyword}`;
    }

    return text;
  }
}