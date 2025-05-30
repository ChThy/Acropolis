/* tslint:disable */
/* eslint-disable */
/**
 * Acropolis.Api | v1
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: 1.0.0
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */


// May contain unused imports in some cases
// @ts-ignore
import type { Resource } from './resource';
// May contain unused imports in some cases
// @ts-ignore
import type { VideoMetaData } from './video-meta-data';

/**
 * 
 * @export
 * @interface DownloadedVideo
 */
export interface DownloadedVideo {
    /**
     * 
     * @type {string}
     * @memberof DownloadedVideo
     */
    'id': string;
    /**
     * 
     * @type {string}
     * @memberof DownloadedVideo
     */
    'url': string;
    /**
     * 
     * @type {VideoMetaData}
     * @memberof DownloadedVideo
     */
    'metaData': VideoMetaData;
    /**
     * 
     * @type {Array<Resource>}
     * @memberof DownloadedVideo
     */
    'resources'?: Array<Resource> | null;
}

