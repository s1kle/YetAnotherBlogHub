export interface blogListVm {
    blogs: blogVmForList[];
}

export interface blogListWithTagsVm {
    blogs: blogVmForListWithTag[];
}

export interface tagListVm {
    tags: tagVmForList[];
}

export interface detailedBlogVm {
    author: string;
    id: string;
    title: string;
    creationDate: Date;
    details: string | null;
    editDate: Date | null;
}

export interface tagVm {
    id: string;
    name: string;
}

export interface blogVmForList {
    author: string;
    id: string;
    title: string;
    creationDate: Date
}

export interface blogVmForListWithTag {
    author: string;
    id: string;
    title: string;
    creationDate: Date
    tags: tagListVm
}

export interface tagVmForList {
    id: string;
    name: string;
}

export interface searchOptions {
    searchQuery: string;
    searchProperties: string;
}

export interface sortOptions {
    sortProperty: string;
    sortDirection: string;
}

export interface createBlogVm {
    title: string;
    details?: string | null;
}

export interface createTagVm {
    name: string;
}

export interface updateBlogVm {
    title: string;
    details?: string | null;
}

export interface createLinkVm {
    tagId: string;
}