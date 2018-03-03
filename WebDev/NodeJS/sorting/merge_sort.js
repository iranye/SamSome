const list = [23,4,42,15,16,8,3];
const = mergeSort = (list) => {
    if (list.length <= 1) return list;
    const middle = list.length / 2;
    const left = list.slice(0, middle);
    const right = list.slice(middle, list.length);
    return merge(mergeSort(left), mergeSort(right));
}
const merge = (left, right) => {
    var result = [];
    while (left.length || right.length) {
        if (left.length)
    }
}
