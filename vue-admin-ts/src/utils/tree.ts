import * as _ from 'lodash'
import { AuditNodeCreateOrEditDto } from '@/api/appService';

export interface INodesBlock {
  items: AuditNodeCreateOrEditDto[]
}

export function createNodes(array: AuditNodeCreateOrEditDto[]
) {
  if (!array)
    return []
  const result: INodesBlock[] = [];
  for (let index = 0; index < array.length; index++) {
    const element = array[index];
    if (!result[element.index!]) {
      result[element.index!] = { items: [] }
    }
    result[element.index!].items.push(element);
  }
  return result;
}

export function flatenNodes(array: INodesBlock[]) {
  const result: AuditNodeCreateOrEditDto[] = [];
  array = array.filter(x => x.items.length > 0);
  for (let i = 0; i < array.length; i++) {
    array[i].items.forEach(x => {
      result.push({ ...x, index: i });
    });
  }
  return result;
}


export function createTableTree(
  array: any[],
  parentIdProperty: string,
  idProperty: string,
  parentIdValue: any,
  childrenProperty: string
) {
  const tree: any[] = []
  const nodes = _.filter(array, [parentIdProperty, parentIdValue])
  _.forEach(nodes, node => {
    const newNode = Object.assign({}, node, { hasChildren: false, isLeaf: true });

    const _children = createTableTree(
      array,
      parentIdProperty,
      idProperty,
      node[idProperty],
      childrenProperty
    )



    if (_children.length > 0) {
      newNode[childrenProperty] = _children
      newNode['hasChildren'] = true
    }
    if (_children.length > 0) newNode['isLeaf'] = false
    tree.push(newNode)
  })
  return tree
}


export function createNgTree(array: any,
  parentIdProperty: any,
  idProperty: any,
  parentIdValue: any,
  childrenProperty: any,
  fieldMappings: any,
  disabled: any) {
  const tree: any[] = []

  const nodes = _.filter(array, [parentIdProperty, parentIdValue])

  _.forEach(nodes, node => {
    const newNode: any = {
      label: node.displayName,
      id: node.name,
      expanded: true,
      isLeaf: true,
      disabled
    }

    // this.mapFields(node, newNode, fieldMappings);

    const _children: any = createNgTree(
      array,
      parentIdProperty,
      idProperty,
      node[idProperty],
      childrenProperty,
      fieldMappings,
      disabled
    )

    newNode[childrenProperty] = _children

    if (_children.length > 0) newNode['isLeaf'] = false
    tree.push(newNode)
  })
  return tree
}

export function createOUTree(array: any,
  parentIdProperty: any,
  idProperty: any,
  parentIdValue: any,
  childrenProperty: any,
  disabled: any) {
  const tree: any[] = []

  let nodes = _.filter(array, [parentIdProperty, parentIdValue])
  _.forEach(nodes, node => {
    const newNode: any = {
      label: `${node.displayName}(${node.memberCount}成员)`,
      data: node,
      id: node[idProperty],
      expanded: true,
      disabled
    }
    newNode[childrenProperty] = createOUTree(
      array,
      parentIdProperty,
      idProperty,
      node[idProperty],
      childrenProperty,
      disabled
    )
    tree.push(newNode)
  })
  return tree
}

export function getKeys(array: any[]) {
  if (array) {
    return Array.from(
      new Set([
        'Pages',
        ..._getKeys(array),
        ...array
      ])
    )
  } // 去重
  return []
}

function _getKeys(array: any[]) {
  let _result: any[] = [];
  [...array].forEach(z => {
    const _index = z.lastIndexOf('.')
    if (_index > 0) {
      const _p = z.substring(0, _index)
      _result = [..._result, _p]
      _result = [..._result, ..._getKeys(_p)]
    }
  })
  return _result
}

export function rebuildKeys(array: any[]) {
  _.remove(array, z => {
    const _l = array.filter(zz => zz.indexOf(z) > -1).length
    return _l > 1
  })
  return array
}
