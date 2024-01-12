export interface blogListVm {
    blogs: blogVmForList[];
}

export interface tagListVm {
    tags: tagVm[];
}

export interface detailedBlogVm {
    author: string;
    userId: string;
    id: string;
    title: string;
    creationDate: Date;
    details: string | null;
    editDate: Date | null;
    tags: tagVm[]
}

export interface tagVm {
    id: string;
    name: string;
}

export interface blogVmForList {
    author: string;
    userId: string;
    id: string;
    title: string;
    creationDate: Date;
    details: string | null;
    editDate: Date | null;
    tags: tagVm[]
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