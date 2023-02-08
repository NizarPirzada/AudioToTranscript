import Vue from "vue";

const GettersHelper = {
  // Find an object in a list of objects by matching a property value.
  // userById: findByKey('users', 'id')
  // getters.userById('123')
  findByKey(prop, targetKey) {
    return (state) => (val) => state[prop].find((x) => x[targetKey] === val);
  },

  // Filter a list of objects by matching a property value.
  // usersByStatus: filterByKey('users', 'status')
  // getters.usersByStatus('INACTIVE')
  filterByKey(prop, targetKey) {
    return (state) => (vals) => {
      if (!Array.isArray(vals)) vals = [vals];
      return state[prop].filter((x) => vals.indexOf(x[targetKey]) > -1);
    };
  },

  mapKeys(prop, targetKey) {
    const filter = GettersHelper.filterByKey(prop, targetKey);
    return (state) => (vals) =>
      filter(state)(vals).sort(
        (a, b) => vals.indexOf(a[targetKey]) - vals.indexOf(b[targetKey])
      );
  },
};

const MutationsHelper = {
  // Find an object in a list of objects by matching a property value.
  // userById: findByKey('users', 'id')
  // getters.userById('123')
  set: (key) => (state, val) => {
    state[key] = val;
  },

  // Set a value at a path within state
  // Create objects and arrays as needed
  // Path is an array, and array indicies are numbers (not string numbers)
  // setUserName: setPath(['user', 'name'])
  // commit('setUserName', 'foo')
  setPath: (path) => (state, val) => {
    const last = (xs) => xs[xs.length - 1];
    const obj = path.slice(0, -1).reduce((acc, x, i) => {
      if (!(x in acc)) acc[x] = typeof path[i + 1] === "number" ? [] : {};
      return acc[x];
    }, state);
    obj[last(path)] = val;
  },

  // Toggle boolean in state
  // toggleOpen: toggle('open')
  // commit('toggleOpen')
  toggle: (key) => (state) => {
    state[key] = !state[key];
  },

  // For all key/value in propMap, set state[key] = data[propMap[value]]
  pick: (propMap) => (state, data) => {
    data = data || {};
    Object.keys(propMap).forEach((x) => {
      state[x] = data[propMap[x]];
    });
  },

  // push an item onto a list
  // addItem: pushTo('items')
  pushTo: (key) => (state, val) => {
    state[key].push(val);
  },

  // copy all key/values from data to state
  // useful for resetting state to default values
  // resetState: assignConstant(initialState)
  // commit('resetState')
  assignConstant: (data) => (state) => {
    Object.assign(state, data);
  },

  // remove item from list
  omitFromList: (key) => (state, item) => {
    const index = state[key].indexOf(item);
    if (index > -1) {
      state[key].splice(index, 1);
    }
  },

  // increment the index of a list argument or a list in state
  incrementListIndex: (key, listOrListProp) => (state) => {
    const list = Array.isArray(listOrListProp)
      ? listOrListProp
      : state[listOrListProp];
    state[key] = (state[key] + 1) % list.length;
  },

  // add or extend a record in a list
  extendRecordInList: (key, idKey = "id", valKey) => (state, data) => {
    const id = data[idKey];
    const val = valKey ? data[valKey] : data;
    const index = state[key].findIndex((x) => x[idKey] === id);
    return index < 0
      ? state[key].push(val)
      : state[key].splice(index, 1, Object.assign({}, state[key][index], val));
  },

  // add or replace a record in a list
  replaceRecordInList: (key, idKey = "id", valKey) => (state, data) => {
    const id = data[idKey];
    const val = valKey ? data[valKey] : data;
    const index = state[key].findIndex((x) => x[idKey] === id);
    return index < 0 ? state[key].push(val) : state[key].splice(index, 1, val);
  },

  // Add or remove value
  addOrRemove: (key) => (state, val) => {
    let index = state[key].indexOf(val);
    let tt;
    let copy = Object.assign([], state[key]);
    if (index < 0) {
      tt = copy.push(val);
    } else {
      tt = copy.splice(index, 1);
    }
    Vue.set(state, key, copy);
    return tt;
  },
};

export { GettersHelper, MutationsHelper };
