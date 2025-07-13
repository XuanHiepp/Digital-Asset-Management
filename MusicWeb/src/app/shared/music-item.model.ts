import { AbstractFormGroupDirective } from '@angular/forms';

export class MusicItem {
    id: string;
    name: string;
    title: string;
    album: string;
    publishingYear: string;
    ownerId: Number;
    licenceId: Number;
    creatureType: string;
    lyricsCheck: boolean;
    audioCheck: boolean;
    mvCheck: boolean;
    ownerType: string;

    isPermanent: boolean;
    isConfirmed: boolean;

    transferId: string;

    key1: string;
    fullKey: string;
    mediaLink: string;
    musicLink: string;

    transactionHash: string;
    contractAddress: string;
    transactionStatus: string;

    dateCreated: string;
}
