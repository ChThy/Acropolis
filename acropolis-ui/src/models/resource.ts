export interface ResourceModel {
  id: string;
  title: string | null;
  description: string;
  url: string;
  source: string;
  viewed: boolean;
  type: 'page' | 'video';
}

export interface Page extends ResourceModel {
  type: 'page';
}

export interface Video extends ResourceModel {
  type: 'video';
}